# RTSS Frame Limiter

RTSSFrameLimiter is a simple UI tool that uses `rtss-cli` to set a frame rate limit for games.

## Preparation

1. **Install .NET 8.0**  
   - You need .NET 8.0 to run this app.  
   - When you launch it, Windows may prompt you to download .NET. If it doesn’t, you can download it manually from:  
     [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download)

2. **Install RivaTuner Statistics Server (RTSS)**  
   - This app requires RTSS to function.  
   - You can download RTSS as part of the MSI Afterburner package from:  
     [https://www.msi.com/Landing/afterburner/graphics-cards](https://www.msi.com/Landing/afterburner/graphics-cards)

## Installation

1. Download the ZIP package from the **Releases** page and extract it to a local folder.  
2. Set both `rtss-cli.exe` and `RTSSFrameLimiter.exe` to run as an administrator:  
   - Right-click each `.exe` file, select **Properties**, go to the **Compatibility** tab, and check **Run this program as an administrator**.

## How to Use

1. Run **RivaTuner Statistics Server**.
2. Run **RTSSFrameLimiter.exe**.
3. In the **Game EXE** text box, enter the executable file name of the game.  
   - Alternatively, you can select it from the dropdown list below the text box.
4. Choose one of the preset FPS buttons or enter a custom FPS value in the **Custom FPS** text box.
5. If using a **Custom FPS** value, click the **Set Custom** button to apply the frame rate limit.

## Credits

- **rtss-cli**: [https://github.com/xanderfrangos/rtss-cli](https://github.com/xanderfrangos/rtss-cli)
