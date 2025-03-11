<template>
  <div>
    <h2>Yeni Ürün Oluştur</h2>
    <form @submit.prevent="submitForm">
      <div>
        <label for="name">Ürün Adı:</label>
        <input v-model="product.name" type="text" id="name" required />
      </div>
      <div>
        <label for="Barcode">Barcode:</label>
        <input v-model="product.Barcode" type="text" id="Barcode" required />
      </div>
      <div>
        <label for="Category">Category:</label>
        <input v-model="product.Category" type="text" id="Category" required />
      </div>
      <div>
        <label for="Location">Location:</label>
        <input v-model="product.Location" type="text" id="Location" required />
      </div>
      <div>
        <label for="CriticalStockLevel">CriticalStockLevel:</label>
        <input v-model="product.CriticalStockLevel" type="number" id="CriticalStockLevel" required />
      </div>
      <button type="submit">Ürün Ekle</button>
    </form>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      product: {
        name: '',
        Barcode: '',
        Category: '',
        CriticalStockLevel: 0,
        Location: '',
      },
    };
  },
  methods: {
    async submitForm() {
      try {
        await axios.post('https://localhost:7079/api/products/CreateProduct', this.product);
        this.$router.push({ name: 'product-list' }); 
      } catch (error) {
        console.error("Ürün eklenemedi:", error);
      }
    },
  },
};
</script>
