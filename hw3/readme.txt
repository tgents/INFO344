Dashboard: http://info344ttseng.cloudapp.net/admin2.html
GitHub: https://github.com/tgents/INFO344/tree/master/hw3


Write Up:

[40pts] Dashboard with relevant data & functionality
	State of each worker role web crawler: click on check status button, retrieved from the stats table
	Machine counters (CPU Utilization%, RAM available): In stats list, retrieved from the stats table
	#URLs crawled: In stats list
	Last 10 URLs crawled: At the bottom of the dashboard, retrieved from the stats table
	Size of queue (#urls left in pipeline to be crawled): In the stats list, retrieved from the stats table
	Size of index (Table storage with our crawled data): In the stats list, retrieved from the stats table
	Any errors and their URLs: at the bottom of the dashboard, queries the error table
[30pts] Worker role to crawl websites for URL, title, date and store to Table storage
	See parseHtml in TomBot.cs it grabs title and date from the page retrieved from the url.
[20pts] Code written in C# – C# best practices!
	I created a separate class that handled parsing html and parsing robots (and xmls) since this functionality is different from the administrative functionality of the worker role.
[10pts] Proper use of worker roles, table and queue storage on Azure
	Queue for commands, queue for html urls, table for errors, table for crawled urls, table for stats. I created a table for stats because I was not sure if I could mix different TableEntities in one table.

	Extra Credit:

	I added graphs for cpu usage and mem availability. Basically just started with a list of zeroes, then as data came in, I used array.shift() to move the data over. Each time I get more data, I wipe and redraw the graph. I used d3 to do this.