namespace SolarWatch.Contracs;

public record LoginRes(AuthResponse Response, IList<string> Role);