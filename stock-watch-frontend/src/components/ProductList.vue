<template>
  <div>
    <h2>Ürün Listesi</h2>
    <table>
      <thead>
        <tr>
          <th>Ürün Adı</th>
          <th>Barcode</th>
          <th>CriticalStockLevel</th>
          <th>Category</th>
          <th>Location</th>
          <th>İşlemler</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="product in products" :key="product.id">
          <td>{{ product.name }}</td>
          <td>{{ product.Barcode }}</td>
          <td>{{ product.CriticalStockLevel }}</td>
          <td>{{ product.Category }}</td>
          <td>{{ product.Location }}</td>
          <td>
            <button @click="editProduct(product.id)">Düzenle</button>
            <button @click="deleteProduct(product.id)">Sil</button>
          </td>
        </tr>
      </tbody>
    </table>
    <button @click="createProduct">Yeni Ürün Ekle</button>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      products: [],
    };
  },
  mounted() {
    this.fetchProducts();
  },
  methods: {
    async fetchProducts() {
      try {
        const response = await axios.get('https://localhost:7079/api/products/AllProducts');
        this.products = response.data;
      } catch (error) {
        console.error("Ürünler alınamadı:", error);
      }
    },
    editProduct(id) {
      this.$router.push({ name: 'update-product', params: { id: id } });
    },
    async deleteProduct(id) {
      try {
        await axios.delete(`https://localhost:7079/api/products/Delete/${id}`);
        this.fetchProducts(); 
      } catch (error) {
        console.error("Ürün silinemedi:", error);
      }
    },
    createProduct() {
      this.$router.push({ name: 'create-product' });
    },
  },
};
</script>
