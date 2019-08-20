## Project under construction :construction:
This project are curently (2019/08/20) under construction.

---

# SHT85 Library
Open source UWP library for communication with Sensirion Humidity Sensor SHT85. 

This library targets __UWP IoT projects__! Download directly from NuGet [Rca.Sht85Lib on NuGet](https://nuget.org/packages/Rca.Sht85Lib).


[![Bulid](https://img.shields.io/appveyor/ci/100prznt/rca-sht85lib.svg?logo=appveyor&style=popout-square)](https://ci.appveyor.com/project/100prznt/rca-sht85lib)   [![Current version](https://img.shields.io/nuget/v/Rca.Sht85Lib.svg?logo=nuget&logoColor=%23ef8b00&style=popout-square)](https://www.nuget.org/packages/Rca.Sht85Lib/)   [![Code size](https://img.shields.io/github/languages/code-size/100prznt/Rca.Sht85Lib.svg?logo=github&style=popout-square)](#) 

### Hardware functionality
* SingleShot support (temperature [°C] and rel. humidity [%RH])
* Periodic data acquisition support incl. timestamp
* Control sensor heater
* Read serial and status-register

### Software features
* Physics calculator for:
	* Dew point [°C]
	* Absolute humidity [%]
	* Vapour-pressure [hPa]
	* Saturation-vapour-pressure [hPa]


## How To install?
Download the source from GitHub or get the compiled assembly from NuGet [Rca.Sht85Lib on NuGet](https://nuget.org/packages/Rca.Sht85Lib).

[![Current version](https://img.shields.io/nuget/v/Rca.Sht85Lib.svg?logo=nuget&logoColor=%23ef8b00&style=popout-square)](https://www.nuget.org/packages/Rca.Sht85Lib/)   [![NuGet](https://img.shields.io/nuget/dt/Rca.Sht85Lib.svg?logo=nuget&logoColor=%23ef8b00&style=popout-square)](https://www.nuget.org/packages/Rca.Sht85Lib/)


## How to use?
Some basic usage examples

### Create an sensor instance
In this example is the I2C address of conneted SHT85 sensor set to default (0x44 factory-fixed):
```cs
var mySht85Sensor = new Rca.Sht85Lib.Sht85();
```

	
### Perform and read measurement
Perform a single reading with default repeatability "low":
```cs
var measData = mySht85Sensor.SingleShot(); //Tuple<double, double>
var temperature = measData.Item1;          //double
var humidity = measData.Item2;             //double
```


### Periodic Data Acquisition Mode
Start the periodic data acquisition mode with specified measure mode:
```cs
void StartPeriodicDataAcquisitionMode()
{
	//...
	mySht85Sensor.NewMeasData += mySht85Sensor_NewMeasData;
	mySht85Sensor.StartPeriodicDataAcquisitionMode(PeriodicMeasureModes.High10Hz);
	//...
}
```
Method to receive the `NewMeasData` event. The update rate depends on the selected measure mode and the hardware runtimes:
```cs
void mySht85Sensor_NewMeasData(Tuple<DateTime, double, double> measData)
{
	var timeStamp = measData.Item1;   //DateTime object
	var temperature = measData.Item2; //double
	var humidity = measData.Item3;    //double
}
```


### Use Physics.Calculator
The library also offers a calculator, which offers the possibility to calculate further sizes.
`measData` is an `Tuple<double, double>` object (Item1: temperature in °C; Item2: rel. humidity in %RH), e.g. provided by `SigleShot()`.
```cs
var myCalculator = new Rca.Sht85Lib.Physics.Calculator();

var dewPoint = myCalculator.DewPoint(measData);           //double
var absHumidity = myCalculator.AbsoluteHumidity(measData) //double
```


## Credits
This library is made possible by contributions from:
* [Elias Rümmler](http://www.100prznt.de) ([@rmmlr](https://github.com/rmmlr)) - core contributor

## License
Rca.Sht85Lib is licensed under [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form"). Refer to [LICENSE.txt](https://github.com/100prznt/Rca.Sht85Lib/blob/master/LICENSE.txt) for more information.

## Contributions
Contributions are welcome. Fork this repository and send a pull request if you have something useful to add.


[![Bulid](https://img.shields.io/appveyor/ci/100prznt/rca-sht85lib.svg?logo=appveyor&style=popout-square)](https://ci.appveyor.com/project/100prznt/rca-sht85lib)


## Related Projects
* [OpenPoolControl](https://github.com/100prznt/opc)
