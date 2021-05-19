import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/ProductType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {

  @ViewChild('search',{ static: false }) searchTerm : ElementRef;
  products: IProduct[];
  types: IType[];
  brands: IBrand[];
  shopParams = new ShopParams();
  totalCount: number;


  sortOptions = [
    {name: 'Alphabetical',value: 'name'},
    {name: 'Price Low to High',value: 'priceAsc'},
    {name: 'Price High to Low',value: 'priceDesc'}
  ];

  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
      this.getProducts();
      this.getBrands();
      this.getTypes();
  }

  getProducts(){
    this.shopService.getProducts(this.shopParams).subscribe((result) => {

      this.products = result.data;
      this.shopParams.pageNumber = result.pageIndex;
      this.shopParams.pageSize = result.pageSize;
      this.totalCount = result.count;
      
    }, error => {
      console.log(error);
    });
  }

  getBrands (){
    this.shopService.getBrands().subscribe(result => {
        this.brands = [{id:0,name:'All'}, ...result];
    }, error => {
      console.log(error);
    });
  }

  getTypes (){
    this.shopService.getTypes().subscribe(result => {
        this.types = [{id:0,name:'All'}, ...result];
    }, error => {
      console.log(error);
    });
  }

  onBrandSelected(brandId:number){
        this.shopParams.brandId = brandId;
        this.shopParams.pageNumber = 1;
        this.getProducts();
  }

  onTypeSelected(typeId:number){
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(sort: string){
      this.shopParams.sort = sort;
      this.getProducts();
  }

  onPageChange(event:any){
    if(this.shopParams.pageNumber !== event){
      this.shopParams.pageNumber = event;
      this.getProducts();
    }
  }

  onSearch(){
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onReset(){
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }


}
