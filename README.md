# MSI Afterburner addon for IOTLink

**Since IOTLink has been discontinued, support for this addon is also discontinued.**

This addon enhances the [IOTLink](https://gitlab.com/iotlink/iotlink) service with telemetry from MSI Afterburner.

## Features
- Multi GPU Support
- GPU Sensors
  - Device Name
  - Driver Family & Version
  - Fan speed (% and RPM) for up to 3 fans.
  - Bus Usage
  - Core Clock
  - Core Usage
  - Core Voltage
  - Core Temperature
  - Core Temperature limit reached (binary)
  - Frame Buffer Usage
  - Memory Clock
  - Memory Usage
  - Power Consumption
  - Power Limit reached (binary)
  - Video Engine Load
 - Enhanced CPU Sensors
  - Clock Speed
  - Power Consumption
  - Core Temperature
- Enhanced Memory Sensors
  - Commit Charge
  - Memory Used
  
## Installation

Simply extract latest release into your IOTLink Addons folder (normally `C:\ProgramData\IOTLink\Addons`) and restart the service, new data points will be collected and displayed.

## Configuration

The `config.yaml` file can be customised to enable or disable certain sensors, or to change the default polling interval.
