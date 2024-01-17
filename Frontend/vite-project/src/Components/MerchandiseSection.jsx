const MerchandiseSection = ({ type, items, addToCart }) => (
  <div>
    <h3>{type}s</h3>
    {items.map((item) => (
      <div key={item.id} className="merchandise-card">
        <p>Type: {item.type}</p>
        {item.size && <p>Size: {item.size}</p>}
        {item.material && <p>Material: {item.material}</p>}
        <p>
          Quantity: {item.quantity}, Price: ${item.price}
        </p>
        <button type="button" onClick={() => addToCart(item)}>
          Add to cart
        </button>
      </div>
    ))}
  </div>
);
export default MerchandiseSection;