<template>
  <div class="stock-updates">
    <div class="buttons">
      <!-- Butonlar -->
      <button @click="goToCreateProduct">Ürün Ekle</button>
      <button @click="goToUpdateProducts(1)">Ürün Güncelle</button>
      <button @click="goToUpdateStock(1)">Stok Güncelle</button>
      <button @click="goToAllProducts">Ürünleri Görüntüle</button>
      <button @click="goToReports">Raporları Görüntüle</button>
    </div>

    <h2>📦 Canlı Stok Güncellemeleri</h2>
    <ul>
      <li v-for="update in stockUpdates" :key="update.productId">
        Ürün ID: <strong>{{ update.productId }}</strong>
      </li>
    </ul>
  </div>
</template>

<script>
import signalrService from "../services/signalrService";

export default {
  data() {
    return {
      stockUpdates: signalrService.stockUpdates,
    };
  },
  methods: {
    
    goToCreateProduct() {
      this.$router.push('/create');
    },
    goToUpdateProducts(id) {
      this.$router.push(`/update/${id}`); 
    },
    
    goToUpdateStock(id) {
      this.$router.push(`/updatestock/${id}`); 
    },
    
    goToAllProducts() {
      this.$router.push('/productlist'); 
    },
    goToReports() {
      this.$router.push('/reports'); 
    }
  }
};
</script>

<style scoped>
.stock-updates {
  background-color: #f8f9fa;
  padding: 15px;
  border-radius: 8px;
  box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.1);
}

h2 {
  color: #007bff;
}

ul {
  list-style: none;
  padding: 0;
}

li {
  padding: 8px;
  border-bottom: 1px solid #ddd;
}

.buttons {
  margin-bottom: 20px;
}

button {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 10px 20px;
  margin-right: 10px;
  border-radius: 5px;
  cursor: pointer;
}

button:hover {
  background-color: #0056b3;
}
</style>
