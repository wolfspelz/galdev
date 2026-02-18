---
name: myreload
description: Reload FUTR data after code or content changes
---

# Reload Data

Trigger a data reload on the local FUTR server after making changes to data files.

## Steps

1. Try to reload data:
```bash
curl -s http://localhost:5000/Reload
```

2. If the reload request fails (connection refused), tell the user to press F5 in VS Code to start the server in debug mode.
