<template>
  <div>
    <h2>Ürün Güncelle</h2>
    <form @submit.prevent="submitForm">
    <div>
        <label for="id">Ürün Id:</label>
        <input v-model="product.id" type="number" id="id" required />
      </div>
      <div>
        <label for="name">Ürün Adı:</label>
        <input v-model="product.name" type="text" id="name" required />
      </div>
      <div>
        <label for="barcode">Barcode:</label>
        <input v-model="product.barcode" type="text" id="barcode" required />
      </div>
      <div>
        <label for="category">Category:</label>
        <input v-model="product.category" type="text" id="category" required />
      </div>
      <div>
        <label for="location">Location:</label>
        <input v-model="product.location" type="text" id="location" required />
      </div>
      <div>
        <label for="criticalStockLevel">CriticalStockLevel:</label>
        <input v-model="product.criticalStockLevel" type="number" id="criticalStockLevel" required />
      </div>
      <button type="submit">Güncelle</button>
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
        barcode: '',
        category: '',
        criticalStockLevel: 0,
        location: '',
      },
    };
  },
  mounted() {
    this.fetchProduct();
  },
  methods: {
    async fetchProduct() {
      const { id } = this.$route.params; 
      try {
        const response = await axios.get(`https://localhost:7079/api/products/OneProduct/${id}`);
        this.product = response.data;
      } catch (error) {
        console.error("Ürün alınamadı:", error);
      }
    },
    async submitForm() {
      //const { id } = this.$route.params; 
      try {
        await axios.put(`https://localhost:7079/api/products/UpdateProduct/${this.product.id}`, this.product);
        this.$router.push({ name: 'product-list' }); 
      } catch (error) {
        console.error("Ürün güncellenemedi:", error);
      }
    },
  },
};
</script>
