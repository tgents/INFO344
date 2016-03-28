<!-- // <?php
// 	if (!empty($_SERVER['HTTPS']) && ('on' == $_SERVER['HTTPS'])) {
// 		$uri = 'https://';
// 	} else {
// 		$uri = 'http://';
// 	}
// 	$uri .= $_SERVER['HTTP_HOST'];
// 	header('Location: '.$uri.'/dashboard/');
// 	exit;
// ?>
// Something is wrong with the XAMPP installation :-(
 -->
 <html>
<head>
<title>Online PHP Script Execution</title>
</head>
<body>
<?php
    echo "Hello World";
    $myBool = TRUE;
    $myInt = 10;
    $myWord = "helloworld";
    $myArray = array("ice cream", "steak", "apples");
    $myKeyValuePair = array("Dad"=>"Joe", "Mom"=>"Amy", "Bro"=>"Jason");
    
    echo "myBool = $myBool\n";
    echo "myInt = $myInt\n";
    echo "myWord = $myWord\n";
    for($i = 0; $i < sizeof($myArray); $i++){
        echo "array[$i] = $myArray[$i]\n";
    }
    foreach($myKeyValuePair as $key=>$value){
        echo "$key = $value\n";
    }
?>
</body>
</html>