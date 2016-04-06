<?php
	include("book.php");
	
	$books = Book::getDefaultBooks();
    foreach($books as $book) {
    	echo $book->getName() . " sells for " . $book->getPrice() . "<br>";
    }
?>