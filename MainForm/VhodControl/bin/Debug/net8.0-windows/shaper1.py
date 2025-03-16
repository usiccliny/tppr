from flask import Flask, request, jsonify
import openpyxl

app = Flask(__name__)

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

        ws.append(selected_columns)

        for row in rows:
            row_data = []
            for col in selected_columns:
                row_data.append(row.get(col))
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