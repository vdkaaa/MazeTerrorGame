// Contrato para guardar/cargar datos de juego
public interface ISaveService
{
    void SaveGame(string slotId);
    void LoadGame(string slotId);
    void DeleteSave(string slotId);
}
