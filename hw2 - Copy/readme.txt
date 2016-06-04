Links:
Search box > http://inf344ttseng.cloudapp.net/fakewiki.html
HTTP GET > http://inf344ttseng.cloudapp.net/TrieService.asmx/SearchTrie?searchString=abes
GitHub > https://github.com/tgents/INFO344/tree/master/hw2

Requirements:
[50pts] Web service works – API returns query suggestions in JSON (fast!)
  I think its pretty fast.
[20pts] Client-side AJAX & modify DOM to show retrieved query suggestions
  Yes it does this. Every time the HTTP request comes back, the JavaScript takes it and updates a list in the html.
[20pts] Web service written in C# – C# best practices!
  I did my best for this. I made objects and stuff.
[10pts] Query suggestion web service runs on Azure
  Query runs on Azure.

Trie:
Each node has its own array of 27 child nodes (a-z and space.) With each letter it will traverse down the tree.
Search will traverse to the end of the given string and then will look for strings in its children. This has its private functions to complete its task.

WebService:
DownloadWiki will download the wiki to the local storage.
BuildTrie will build the trie by reading each line and adding to the trie.
Search will call the trie search with the given string.