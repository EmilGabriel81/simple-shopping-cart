import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Product } from '../model/product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http: HttpClient) { }

  private readonly url = 'assets/products.json';

    loadProducts(): Observable<Product[]> {
      return this.http.get<Product[]>(this.url);
      }
}
