const productsIds = [];

function AddToCart(productId) {
    productsIds.push(productId);
    console.log(productsIds);
}

async function postData(url = '') {
    const response = await fetch(url);
    return await response.text();
}


async function ChangeCategory(category) {
    const container = document.getElementById("container");
    await postData(`filter/category/${category}`)
        .then((data) => {
            container.innerHTML = data;
        });
}

async function ChangeSupplier(supplier) {
    const container = document.getElementById("container");
    await postData(`filter/supplier/${supplier}`)
        .then((data) => {
            container.innerHTML = data;
        });
}
// se trimite in body cu post cand ai val mare
//get e default 
//post 