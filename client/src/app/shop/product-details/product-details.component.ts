import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute, private basketService: BasketService) { }
  product: IProduct;
  quantity = 1;

  ngOnInit(): void {

    this.loadProduct();
  }

  addItemToBasket(){
    this.basketService.addItemToBasket(this.product, this.quantity);
  }


  incrementQuantity(){
    this.quantity ++;
  }

  decrementQuantity(){
    if(this.quantity > 1)
    this.quantity --;
  }

  loadProduct(){
    this.shopService.getProduct(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(result => {

        this.product = result;

    }, error => {
      console.log(error);
    })
  }
}
