# WebServerScanner
Scan a network segment for webservers and retrieve the root page

## Goal
A simple tool to scan a network segment for webservers and get the headers and ping data from those hosts. 
This is to be used in a segment where for example routers or IoT devices have a status page running on a certain port which you 
want to be able to quickly scan and access.

## What's to be expected
Code will be expanded to have a UI and a Terminal implementation that will allow you to scan a network range (if possible based on your 
network details). The application will show which IP's are used (pingable) and which are hosting one or more webservers on the scan ports. 
Tool will also allow to do a "deep" scan where a failed Ping will not stop us from trying ports for Http(s) (e.g. when Ping is disabled on a machine).

## Disclaimer
Not to be used by bad people with evil plans. Idea sprouted from IoT stuff at my employer where I wanted to have a simple tool to be able to scan
the segment for our IoT device status pages.
