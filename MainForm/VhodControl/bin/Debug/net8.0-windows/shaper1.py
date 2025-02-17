from flask import Flask, request, jsonify
import openpyxl

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

        wb = openpyxl.Workbook()
        ws = wb.active

        headers = [col for col in selected_columns if col in key_mapping.values()]
        ws.append(headers)

        for row in rows:
            row_data = []
            for col in selected_columns:
                for key, value in key_mapping.items():
                    if value == col:
                        row_data.append(row.get(key, None))
                        break
            ws.append(row_data)

        file_name = "output.xlsx"
        wb.save(file_name)

        responseContent = {
            "row_count": len(rows),
            "selected_columns": selected_columns
        }

        return jsonify({"message": "Excel file created", "responseContent": responseContent}), 200

    except Exception as e:
        return jsonify({"error": str(e)}), 500

if __name__ == '__main__':
    app.run(port=5000)