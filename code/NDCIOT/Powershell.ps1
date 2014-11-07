$socket = new-object System.Net.Sockets.TcpClient("127.0.0.1", 4502)
$stream = $socket.GetStream() 
$writer = new-object System.IO.StreamWriter $stream
$writer.WriteLine("TextProtocol")
$writer.Flush();

#Encoding
$enc = [System.Text.Encoding]::UTF8

$content = $enc.GetBytes("chatstate|set_color|blue")
[byte[]]$start = new-object byte[] 1
$start[0] = 0x00
[byte[]]$end = new-object byte[] 1
$end[0] = 0xFF
$mergedArray = $start + $content + $end 

#Set color
$stream.Write($mergedArray,0,$mergedArray.Length)

#Read buffer
[byte[]]$buffer = new-object byte[] 1024

while($true)
{   
	$read = $stream.Read($buffer, 0, 1024)    
	Write-Host($enc.GetString($buffer, 0, $read))
	## Allow data to buffer for a bit 
	start-sleep -m 100
}