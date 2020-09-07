# amk_dyno
an application to control an amk inverter through a usb to can adapter to control an amk pmsm
this is supposed to be used on a dyno to simulate the restriction to 80kW of power drawn from the battery of a formula student electric car.
the software calculates the requested torque to send to the inverter by can based on this power restriction and whatever algorithm the user wants to test.

the program is a dotnet windows forms C# application which uses the c# api of the can adapter that was at hand at creation.
the gui should include various output values that the inverter reports back (rpm, temp, errors...), as well as the currently requested torque.
the program should also be able to log all of the important values to a location and at a rate that can be specified by the user.
the gui should allow to specify this parameter and path as well as a number of useful parameters that can be used to vary the algorithm that is running.
