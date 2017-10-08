import { Component, Input, OnInit } from '@angular/core';
import { ProductItem } from '../../models/product-item';

@Component({
    moduleId: module.id.toString(),
    selector: 'product-grid',
    templateUrl: 'product-grid.component.html',
    styleUrls: ['product-grid.component.css']
})
export class ProductGridComponent implements OnInit {
    public products: ProductItem[];

    constructor () {
        this.products = [];
    }
    
    ngOnInit(): void {
        // TODO: move to a service and wire with BE
        this.products.push({name: "item 1", description: "pretty long descriptioooooooooooooooooooooooooooon nsdfasfas sagasfg asgaksgalsg asgkasgsjag askljg asjgkjasgjasflasdfl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 25, pictureUrl: ""});
        this.products.push({name: "item 2", description: "pretty long nsdfasfas sagasfg asgaksgalsg asgkasgsjag a dfjvb sdjv jd", price: 252, pictureUrl: ""});
        this.products.push({name: "item 3", description: "pretty long descriptioooooooooooooooooooooooooooon nsdfasfas sagasfg asgaksgalsg asgjasflasdfl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 225, pictureUrl: ""});
        this.products.push({name: "item 4", description: "pretty long  nsdfasfas sagasfg asgaksgalsg asgkasgsjag askljg asjgkjasgjasflasdfl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 255, pictureUrl: ""});
        this.products.push({name: "item 5", description: "pretty long descriptioooooooooooooooooooooooooooon nsdfasfas sagasfg asgaksgalsg asgkasgsjag askljg asjgkjasgjasflasdfl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 275, pictureUrl: ""});
        this.products.push({name: "item 6", description: "pretty long descriptiooooooooooooooooooooooooosflasdfl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 5, pictureUrl: ""});
        this.products.push({name: "item 7", description: "pretty long descriptioooooooooooooooooooojag askljg asjgkjasgjasflasdfl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 75, pictureUrl: ""});
        this.products.push({name: "item 8", description: "pg asgaksgalsg asgkasgsjag askljg asjgkjasgjasflasdfl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 85, pictureUrl: ""});
        this.products.push({name: "item 9", description: "pretty long descriptioooooooooooooooooooooooooooon nsdfasfas sagasfg asgaksgalsg asgasflasdfl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 55, pictureUrl: ""});
        this.products.push({name: "item 10", description: "prettnsdfasfas sagasfg asgaksgalsg asgkasgsjag askljg asjgkjasgjasflasdfl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 25, pictureUrl: ""});
        this.products.push({name: "item 11", description: "pretty long descriptiooooooooooosdfasfas sagasfg asgaksgalsg asgkasgsjag askljgl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 95, pictureUrl: ""});
        this.products.push({name: "item 12", description: "pretty long descriptioooooooooooooooooooooooooooon nsaskljg asjgkjasgjasflasdfl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 75, pictureUrl: ""});
        this.products.push({name: "item 13", description: "pretty long descriptiooooooooon nsdfasfas sagasfg asgaksgalsg asgkasgsjag askljg asjgkjasgjasflasdflf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 51, pictureUrl: ""});
        this.products.push({name: "item 14", description: "pretty long descriptioooooooooooooooooooooooooooon nsdfasfas g asgaksgalsg asgkasgsjaggkjasgjasflasdfl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 345, pictureUrl: ""});
        this.products.push({name: "item 15", description: "pretty long descriptiooooooooooooon gasfg asgaksgalsg asgkasgsjag askljg asjgkjasgjasflasdfl ksfgoehriuf geirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 2345, pictureUrl: ""});
        this.products.push({name: "item 16", description: "pretty long descriptiooooooooooooooooooooooon nsdasfas sagasfg asgaksgalsg asgkasgsjag askljg asjgkjasgjasflasdfl ksfgoehriuf bvjhsdbv jsdvj dfjvb sdjv jd", price: 36, pictureUrl: ""});
        this.products.push({name: "item 17", description: "pretty long descriptioooooooooooooooooooooooooooonsfas sagasfg asgaksgalsg asgkasgsjag askljg asjgkjasgjasflasdirf ef e dhfvjhdfvjhds bvjhsdbv jsdvj dfjvb sdjv jd", price: 2345, pictureUrl: ""});
    }
}