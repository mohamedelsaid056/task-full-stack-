import { Component } from '@angular/core';
import { ProductnewService } from '../home/productnew.service';
import { Product } from '../home/Product';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  products: Product[] = [];
  constructor(private productService: ProductnewService) {}

  ngOnInit() {
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getProducts().subscribe((products) => {
      this.products = products;
    });
  }

  deleteProduct(id: number | undefined) {
    if (!id) return;

    this.productService.deleteProduct(id).subscribe(() => {
      this.loadProducts();
    });
  }

  updateProduct(product: Product) {
    this.productService.updateProduct(product.id!, product).subscribe(() => {
      this.loadProducts();
    });
  }

  createProduct(product: Product) {
    this.productService.createProduct(product).subscribe(() => {
      this.loadProducts();
    });
  }
}
