const connection = new signalR.HubConnection('/chathub');

connection.on('ReceiveMessage', (timestamp, user, message) => {
	//const encodedUser = user;
	//const encodedMsg = message;
	//const listItem = document.createElement('li');
	//listItem.innerHTML = ' <b>' + encodedUser + '</b>: ' + encodedMsg +" "+ timestamp;
	//document.getElementById('messages').appendChild(listItem);
	bindMsg();
});

document.getElementById('send').addEventListener('click', event => {

	const msg = document.getElementById('message').value;
	const usr = document.getElementById('user').value;

	var e = document.getElementById("UserId");
	var id = e.options[e.selectedIndex].value;

	$.ajax({
		url: '../../Home/saveMsg',
		type: 'GET',
		data: {
			empid: usr,
			msgtext: msg,
			receiver: id
		},
		contentType: 'application/json;charset=utf-8'
	}).done(function (data) {
	
		connection.invoke('SendMessage', usr, msg).catch(err => showErr(err));
		event.preventDefault();

	}).fail(function (status) {
		result = "Could not communicate with the server.";
	});
	//connection.invoke('SendMessage', usr, msg).catch(err => showErr(err));
	event.preventDefault();
});

function showErr(msg) {

	const listItem = document.createElement('li');
	listItem.setAttribute('style', 'color: red');
	listItem.innerText = msg.toString();
	document.getElementById('messages').appendChild(listItem);
}

connection.start().catch(err => showErr(err));

document.getElementById('UserId').addEventListener('change', event => {

	bindMsg();
	//connection.invoke('SendMessage', usr, msg).catch(err => showErr(err));
	//event.preventDefault();
});

function bindMsg() {
	const usr = document.getElementById('user').value;

	var e = document.getElementById("UserId");
	var id = e.options[e.selectedIndex].value;
	var result = "";
	$.ajax({
		url: '../../Home/GetMsg',
		type: 'GET',
		data: {
			empid: usr,
			receiver: id
		},
		contentType: 'application/json;charset=utf-8'
	}).done(function (data) {

		result = data;
		document.getElementById('messages').innerHTML = "";
		for (var i = 0; i < result.length; i++) {
			const listItem = document.createElement('li');
			listItem.innerHTML = result[i].msgText;
			document.getElementById('messages').appendChild(listItem);
		}

	}).fail(function (status) {
		result = "Could not communicate with the server.";
	});
}



//document.getElementById('send').addEventListener('click', event => {

//	const msg = document.getElementById('message').value;
//	const usr = document.getElementById('user').value;

//	var e = document.getElementById("UserId");
//	var id = "kesha.shah@gmail.com";// e.options[e.selectedIndex].value;

//	$.ajax({
//		url: '../../Home/saveMsg',
//		type: 'GET',
//		data: {
//			empid: usr,
//			msgtext: msg,
//			receiver: id
//		},
//		contentType: 'application/json;charset=utf-8'
//	}).done(function (data) {
//		connection.invoke('SendMessage', usr, msg).catch(err => showErr(err));
//		event.preventDefault();

//	}).fail(function (status) {
//		result = "Could not communicate with the server.";
//	});

//	//connection.invoke('SendMessage', usr, msg).catch(err => showErr(err));
//	//event.preventDefault();
//});