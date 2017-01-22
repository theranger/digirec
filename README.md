This project was a quick hack to create the schedule-based digital television UDP signal recorder.
It was mainly developed for recording unencrypted digital multicast streams from Telia DigiTV network in Estonia.

This recorder works as a Windows service. It reads a text-based schedule file that contains information about television shows to be saved. When defined time arrives, the service joins to an UDP multicast group of the channel and saves received TS-stream into local file.
Service can be installed via console that has administrative privileges.

Schedule file is called `schedule.txt` and must be saved into the same folder next to service executable. File has following format:
```
	<startDate>\t<endDate>\t<udpBroadCastaddress>\t<localFileName>
```

Service must be manually allowed through local firewall, otherwise the file is created but the data cannot be retrieved and file size will remain 0.

This is a source release only, no precompiled binaries are available.
