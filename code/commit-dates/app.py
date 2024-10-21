import os
from git import Repo

# Path to your local Git repository (you can clone it if it's not local yet)
repo_path = 'C:\\Heiner\\github-galdev'  # Change this to your local repo path
data_folder = os.path.join(repo_path, 'data')

# Initialize the repo object
repo = Repo(repo_path)

# Function to get the first and latest commit date for a file
def get_commit_dates(repo, file_path):
    # Get the commits affecting the file, sorted by date
    commits = list(repo.iter_commits(paths=file_path))
    if commits:
        # First commit is the last one in the list, and the latest is the first one
        first_commit_date = commits[-1].committed_datetime
        last_commit_date = commits[0].committed_datetime
        return first_commit_date, last_commit_date
    else:
        return None, None

# Iterate over all files in the data folder
for root, dirs, files in os.walk(data_folder):
    for file in files:
        if file.endswith('.yaml'):
            # Full path to the file
            file_path = os.path.join(root, file)
            # Get the relative path from the repo root
            relative_file_path = os.path.relpath(file_path, repo_path)
            # Get the first and last commit dates for this file
            first_commit_date, last_commit_date = get_commit_dates(repo, relative_file_path)
            if first_commit_date and last_commit_date:
                # Format the dates to "YYYY-MM-DD"
                first_commit_date_str = first_commit_date.strftime('%Y-%m-%d')
                last_commit_date_str = last_commit_date.strftime('%Y-%m-%d')
                print(f"{relative_file_path} {first_commit_date_str} {last_commit_date_str}")
            else:
                print(f"{relative_file_path}: No commit history found")

