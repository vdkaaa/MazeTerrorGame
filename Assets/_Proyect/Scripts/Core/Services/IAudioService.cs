// Contrato para reproducir efectos de sonido y música /No me gusta contrato pero es lo que hace la interfaz:/
public interface IAudioService
{
    void PlaySFX(string clipId);
    void PlayMusic(string trackId);
    void StopMusic();
}
