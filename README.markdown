# Twarchive - Archive Your Twitter Updates

This is a simple command line tool which can be run to download your tweets to a file.

For example, running "twarchive saqibs" will download all tweets by me into the file Twarchive.saqibs.txt in your "My Documents" folder. Running the same command again will only download new tweets. Therefore, you could put this command in a Scheduled Task to run every day.

The output is a tab-separated file containing three columns: ID, Text, and Timestamp. This file could be loaded into Excel, or any other program for viewing or further processing. Remember not to change the twarchive.username.txt file itself, as this file will be updated by Twarchive when next run.

I wrote this tool for my own use, and am still ironing out a few bugs - bear this in mind if you choose to download it. Feel free to tweet me @SaqibS.
