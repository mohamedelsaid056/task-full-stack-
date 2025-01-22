import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductnewService } from '../home/productnew.service';
import { Product } from '../home/Product'; // Import the Product interface

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.scss'],
})
export class EditProductComponent implements OnInit {
  product: Product = { id: 0, title: '', description: '' }; // Initialize with default values

  constructor(
    private route: ActivatedRoute,
    private productNewService: ProductnewService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const productId = this.route.snapshot.paramMap.get('id');
    if (productId) {
      this.productNewService.getProduct(+productId).subscribe((product) => {
        this.product = product; // Ensure product has the correct type
      });
    } else {
      console.error('Product ID is null or undefined');
      // Handle the error case, e.g., navigate back or show a message
    }
  }

  saveProduct() {
    if (this.product.id !== undefined) {
      this.productNewService
        .updateProduct(this.product.id, this.product)
        .subscribe({
          next: (response) => {
            console.log('Product updated successfully:', response);
            this.router.navigate(['/home']);
          },
          error: (error) => {
            console.error('Error updating product:', error);
            // Handle error case - you might want to show an error message to the user
          },
        });
    } else {
      console.error('Product ID is undefined, cannot save.');
    }
  }
}
