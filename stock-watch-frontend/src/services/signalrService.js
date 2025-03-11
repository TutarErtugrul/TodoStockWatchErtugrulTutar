import * as signalR from "@microsoft/signalr";
import { ref } from "vue";

const stockUpdates = ref([]);

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7079/stockHub", {
    withCredentials: true, 
    transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling
  })
  .withAutomaticReconnect()
  .build();

const startConnection = async () => {
  try {
    await connection.start();
    console.log("SignalR bağlantısı başarılı");

    connection.on("ReceiveStockUpdate", (productId, newStock) => {
      console.log(`Güncellenen stok: ${productId} - ${newStock}`);
      const existingProductIndex = stockUpdates.value.findIndex(item => item.productId === productId);
      if (existingProductIndex !== -1) {
        
        stockUpdates.value[existingProductIndex].newStock = newStock;
      } else {
        
        stockUpdates.value.push({ productId, newStock });
      }
    });
  } catch (error) {
    console.error("SignalR bağlantı hatası:", error);
  }
};

startConnection();

export default {
  stockUpdates,
};
