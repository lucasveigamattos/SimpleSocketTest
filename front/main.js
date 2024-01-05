const webSocket = new WebSocket("ws://localhost:5033/api/socket/connect-socket")
const messages = []

webSocket.onmessage = async (event) => {
    const data = await event.data.text()
    messages.push(data)
    console.clear()

    for (const message of messages) {
        console.log(message)
    }
}

const inpMessage = document.getElementById("inpMessage")
document.getElementById("btnSendMessage").addEventListener("click", async () => {
    await fetch("http://localhost:5033/api/socket/send-message", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            message: inpMessage.value
        })
    })

    inpMessage.value = ""
})