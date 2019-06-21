# Beacon_Autostart.sh
# Script to autostart beacon transmission on Raspberry Pi

# Note : remove sudo when copying to /etc/rc.local


sudo hciconfig hci0 up
sudo hciconfig hci0 leadv 3

# Choose one from 3 different available beacon ID

sudo hcitool -i hci0 cmd 0x08 0x0008 1E 02 01 06 1A FF 4C 00 02 15 C7 C1 A1 BF BB 00 4C AD 87 04 9F 2D 29 17 DE D2 00 00 00 00 C8 00
#sudo hcitool -i hci0 cmd 0x08 0x0008 1E 02 01 06 1A FF 4C 00 02 15 98 37 4D 0A FA 8F 43 AB 96 8B 88 EA F8 3C 6E 4C 00 00 00 00 C8 00
#sudo hcitool -i hci0 cmd 0x08 0x0008 1E 02 01 06 1A FF 4C 00 02 15 94 CA DE 28 D4 E6 44 E3 85 4A CA 1C 98 27 72 81 00 00 00 00 C8 00