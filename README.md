# WifiDump
Wireless Network SSID and Password dumper for Windows.

The executable will run silently (no indication other than cursor loading icon), and will create a .txt in the directory
in which it is run, containing all Wireless Network SSIDs and their respective passwords that the host machine has been
connected to.

The naming format for the dump text file is:
SSID_PASS_DUMP_((Day)-(Month))_((Hour)-(Minute)).txt

While running, the program creates an 'tempSSIDs.txt' file, which is contains only the SSIDs and not respective passwords.
This will be deleted if the program runs successfully, being replaced instead by the SSID_PASS_DUMP text file.

Note: Make sure that the respective network card is attached to the computer, or else no dump file will be created.
