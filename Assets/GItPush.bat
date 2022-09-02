@echo off
echo %cd%
git status
SET /p "message=enter a commit message: "
echo "%message%"
git add *
git commit -m "%message%"
git push
SET /p "dummy=done"