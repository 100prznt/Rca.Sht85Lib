## Project under construction :construction:
This project are curently (2019/08/16) under construction.

---

# Sht85Lib
Open source UWP library for communication with Sensirion Humidity Sensor SHT85. 

This library targets __UWP IoT projects__! Download directly from NuGet [Rca.Sht85Lib on NuGet](https://nuget.org/packages/Rca.Sht85Lib).

* SingleShot support (temperature and humidity)


[![Bulid](https://img.shields.io/appveyor/ci/100prznt/rca-sht85lib.svg?logo=appveyor&style=popout-square)](https://ci.appveyor.com/project/100prznt/rca-sht85lib)   [![Current version](https://img.shields.io/nuget/v/Rca.Sht85Lib.svg?logo=nuget&logoColor=%23ef8b00&style=popout-square)](https://www.nuget.org/packages/Rca.Sht85Lib/)   [![Code size](https://img.shields.io/github/languages/code-size/100prznt/Rca.Sht85Lib.svg?logo=github&style=popout-square)](#) 


## How to use?
Some basic usage examples

### Create an sensor instance
In this example is the I2C address of conneted SHT85 sensor set to default (0x44):
```cs
var mySht85Sensor = new Sht85();
```

	
### Perform and read measurement
Perform a single reading with default repeatability "low":
```cs
Tuple<double, double> measData = mySht85Sensor.SingleShot();
double temperature = measData.Item1;
double humidity = measData.Item2;
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
Method to receive the `NewMeasData` event:
```cs
void mySht85Sensor_NewMeasData(Tuple<DateTime, double, double> measData)
{
	DateTime timeStamp = measData.Item1;
	double temperature = measData.Item2;
	double humidity = measData.Item3;
}
```


## How To install?
Download the source from GitHub or get the compiled assembly from NuGet [Rca.Sht85Lib on NuGet](https://nuget.org/packages/Rca.Sht85Lib).

[![Current version](https://img.shields.io/nuget/v/Rca.Sht85Lib.svg?logo=nuget&logoColor=%23ef8b00&style=popout-square)](https://www.nuget.org/packages/Rca.Sht85Lib/)   [![NuGet](https://img.shields.io/nuget/dt/Rca.Sht85Lib.svg?logo=nuget&logoColor=%23ef8b00&style=popout-square)](https://www.nuget.org/packages/Rca.Sht85Lib/)

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
