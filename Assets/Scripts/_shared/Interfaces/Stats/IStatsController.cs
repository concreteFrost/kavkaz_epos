using System.Collections.Generic;

public interface IStatsController
{
    IStatModel GetRequiredModel(StatType type);

}