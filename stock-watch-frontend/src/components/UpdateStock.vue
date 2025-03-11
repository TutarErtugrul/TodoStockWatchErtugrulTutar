<template>
  <div>
    <h2>Ürün Stok Güncelleme</h2>
    <form @submit.prevent="updateStock">
      <div>
        <label for="Id">Ürün ID:</label>
        <input type="number" v-model="stockUpdateData.Id" id="Id" required />
      </div>
      
      <div>
        <label for="StockQuantity">Değişen Stok Miktarı:</label>
        <input type="number" v-model="stockUpdateData.StockQuantity" id="StockQuantity" required />
      </div>

      <div>
        <label for="OperationType">İşlem Türü:</label>
        <select v-model="stockUpdateData.OperationType" id="OperationType" required>
          <option value="Add">Ekle</option>
          <option value="Sale">Satış</option>
        </select>
      </div>

      <button type="submit">Güncelle</button>
    </form>

    <div v-if="message" class="message">
      <p>{{ message }}</p>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      productId: null,
      stockUpdateData: {
        id: 0,
        StockQuantity: 0,
        OperationType: 'Add' 
      },
      message: ''
    };
  },
  methods: {
    async updateStock() {
      const token = localStorage.getItem("userToken"); 
      
      try {
    const response = await axios.put(`https://localhost:7079/api/products/UpdateStock/${this.stockUpdateData.Id}`, {
      ...this.stockUpdateData 
    }, {
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}` 
      }
    });

    console.log("Ürün başarıyla güncellendi:", response.data);
  } catch (error) {
        this.message = `Stok güncelleme başarısız: ${error.response ? error.response.data : error.message}`;
      }
    }
  }
};
</script>

<style scoped>
form {
  max-width: 400px;
  margin: 20px auto;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 8px;
}

div {
  margin-bottom: 15px;
}

label {
  display: block;
  margin-bottom: 5px;
}

input,
select {
  width: 100%;
  padding: 8px;
  margin-top: 5px;
  border-radius: 4px;
  border: 1px solid #ccc;
}

button {
  width: 100%;
  padding: 10px;
  background-color: #4CAF50;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 16px;
  cursor: pointer;
}

button:hover {
  background-color: #45a049;
}

.message {
  margin-top: 20px;
  padding: 10px;
  background-color: #f0f0f0;
  border: 1px solid #ccc;
  border-radius: 4px;
}
</style>
