public interface ITakePositionData{
    public void AddNewPositionData(SendData data);
    public bool ReadyToRecieve { get; }

    public void EndProces();
}

