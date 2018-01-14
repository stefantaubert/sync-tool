# Description
Tool for synchronizing two directories 1:1.
- File operations:
	- if file only existis in origin directory then it will be copied
	- if file in destination not existis in origin then it will be deleted
	- if file exists in both it will look if the last write time is not equal or if the size of the files are different, then it will overwrite the destination file. In all cases all attributes will be applied:
		- last access time
		- last write time 
		- creation time 
		- attributes (e.g. hidden)
- Directory operations:
	- if directory only existis in origin directory then it will be created in destination
	- if directory in destination not existis in origin then it will be deleted
	- if directory exists in both all directory properties from origin will be applied
		- last access time
		- last write time 
		- creation time 
		- attributes (e.g. hidden)
		
## Getting Started
Either start the console application and enter the origin directory and destination directory or:
1. create file `settings.txt` in application dir.
2. write the origin directory in the first line.
3. write the destination directory in the second line.

Example of settings.txt
```
C:\
D:\
```

## Screenshots

<p align="center">
  <img src="/screenshots/console.png">
</p>

# Acknoledgements
Icon: https://www.iconfinder.com/icons/98003/center_sync_icon