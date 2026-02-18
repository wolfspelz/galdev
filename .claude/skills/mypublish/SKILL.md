---
name: mypublish
description: Rebase deployment branch onto main and push to trigger deployment
---

# Deploy

Rebase the `deployment` branch onto `main` and push to trigger the deployment pipeline.

## Steps

1. Push `main` to origin
2. Fetch latest from origin
3. Checkout `deployment` branch
4. Rebase onto `main`
5. Force push with lease
6. Switch back to `main`

## Command

```bash
git push origin main && git fetch origin && git checkout deployment && git rebase main && git push --force-with-lease && git checkout main
```
