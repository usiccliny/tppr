from flask import Flask, request, jsonify
import pandas as pd

app = Flask(__name__)

key_mapping = {
    "order_order_date": "Дата и время заказа",
    "order_customer_name": "Имя клиента, который сделал заказ",
    "order_quantity": "Количество заказанных булочек",
    "bun_name": "Название булочки",
    "bun_price": "Цена булочки",
    "category_category_name": "Название категории булочек",
    "ingredient_ingredient_name": "Название ингредиента",
    "recipe_quantity": "Количество данного ингредиента в рецепте"
}

@app.route('/create_excel', methods=['POST'])
def create_excel():
    try:
        data = request.json
        selected_columns = data.get('SelectedColumns', [])
        rows = data.get('Rows', [])

        if not selected_columns:
            return jsonify({"error": "No columns selected"}), 400

        df_data = {key_mapping[col]: [] for col in selected_columns if col in key_mapping}

        for row in rows:
            for col in selected_columns:
                if col in key_mapping:
                    df_data[key_mapping[col]].append(row.get(col))

        df = pd.DataFrame(df_data)

        file_name = "output.xlsx"
        df.to_excel(file_name, index=False)

        return jsonify({"message": f"Excel file created: {file_name}"}), 200

    except Exception as e:
        return jsonify({"ror": str(e)}), 500

if __name__ == '__main__':
    app.run(port=5000)