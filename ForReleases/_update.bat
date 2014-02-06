@echo off

Rem Make sure we have all of the parameters we need
Rem 1 = process id of Vixen+
Rem 2 = Downloaded file path and file name
Rem 3 = Vixen+ path

if [%1]==[] goto parmError
if [%2]==[] goto parmError
if [%3]==[] goto parmError

Rem Looks okay, start updating
@Echo Waiting for Vixen+ to close...

Rem Loop until the process id that is Vixen+ ends.  This presumes there is only one instance.
:loop
tasklist /FI "pid eq %1" >vixen__2.txt 2>&1
find /I /N "INFO: No" vixen__2.txt > NUL
if not "%ERRORLEVEL%"=="0" goto loop
del vixen__2.txt

Rem Okay Vixen+ is closed, call 7zr.exe with the right flags and parameters
cd %3
7zr x -y %2
 
Rem Remove the .7z file
del %2
goto exit

Rem OOPS something happened or this was run by the user
:parmError
@echo There was a paramater error, can not update.

:exit
Rem Launch Vixen+ and Exit
start /b VixenPlus