# Planner
[![.NET Core Desktop](https://github.com/andrew-pineiro/Planner/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/andrew-pineiro/Planner/actions/workflows/dotnet-desktop.yml)

## Summary
Simple planning of your daily schedule all in one application with very little overhead. Stores data in a `.csv` file, which is specified in the configuration.

## Configuration
App.Config contains multiple keys that are required to run the program:
- `dataFile` - Source file for data, acts as the database for the program
- `purgeMonths` - # of days to purge backup files

### Sample

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <add key="dataFile" value="path\to\data.csv"/>
    <add key="purgeMonths" value="# of months" />
  </appSettings>
</configuration>
```

## Contributors
- Andrew Pineiro
