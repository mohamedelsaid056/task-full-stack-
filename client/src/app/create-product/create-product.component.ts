import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductnewService } from '../home/productnew.service';
import { Product } from '../home/Product';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.scss'],
})
export class CreateProductComponent implements OnInit {
  productForm!: FormGroup;
  submitted = false;
  loading = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private productService: ProductnewService
  ) {}

  ngOnInit(): void {
    this.productForm = this.formBuilder.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  get f() {
    return this.productForm.controls;
  }

  onSubmit() {
    this.submitted = true;

    if (this.productForm.invalid) {
      return;
    }

    this.loading = true;
    const newProduct: Product = {
      id: 0, // The API will assign the actual ID
      title: this.productForm.value.title,
      description: this.productForm.value.description,
    };

    this.productService.createProduct(newProduct).subscribe({
      next: (response) => {
        console.log('Product created successfully', response);
        this.router.navigate(['/']);
      },
      error: (error) => {
        console.error('Error creating product:', error);
        this.loading = false;
      },
    });
  }
}
