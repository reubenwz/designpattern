using System;

public interface ITimeValidator
{
    bool ValidateTime(MissionPlan mission);
    string ValidityPeriod { get; set; }
}

public interface IAltitudeValidator
{
    bool ValidateAltitude(MissionPlan mission);
    string ValidityPeriod { get; set; }

}

// Time Validator Implementation
public class TimeValidator : ITimeValidator
{
    public string ValidityPeriod { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool ValidateTime(MissionPlan mission)
    {
        Console.WriteLine("Validating mission time...");
        return mission.StartTime >= DateTime.Now;
    }
}

// Altitude Validator Implementation
public class AltitudeValidator : IAltitudeValidator
{
    public string ValidityPeriod { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool ValidateAltitude(MissionPlan mission)
    {
        Console.WriteLine("Validating altitude...");
        return mission.Altitude >= 300;
    }
}

// Composite Validator using multiple inheritance through interfaces
public class MissionValidator : ITimeValidator, IAltitudeValidator
{
    private readonly ITimeValidator _timeValidator;
    private readonly IAltitudeValidator _altitudeValidator;

    public string ValidityPeriod { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public MissionValidator(ITimeValidator timeValidator, IAltitudeValidator altitudeValidator)
    {
        _timeValidator = timeValidator;
        _altitudeValidator = altitudeValidator;
    }

    public bool ValidateTime(MissionPlan mission) => _timeValidator.ValidateTime(mission);

    public bool ValidateAltitude(MissionPlan mission) => _altitudeValidator.ValidateAltitude(mission);
}

// Example mission plan
public class MissionPlan
{
    public DateTime StartTime { get; set; }
    public int Altitude { get; set; }
}

class Program
{
    static void Main()
    {
        MissionPlan mission = new MissionPlan { StartTime = DateTime.Now.AddMinutes(10), Altitude = 350 };

        var missionValidator = new MissionValidator(new TimeValidator(), new AltitudeValidator());

        bool isTimeValid = missionValidator.ValidateTime(mission);
        bool isAltitudeValid = missionValidator.ValidateAltitude(mission);

        Console.WriteLine($"Mission Time Valid: {isTimeValid}, Altitude Valid: {isAltitudeValid}");
    }
}
