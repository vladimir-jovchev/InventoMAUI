namespace Invento.MAUI.Views;

public class DashboardPage : ContentPage {
	public DashboardPage() {
		InitializeComponent();
		LoadDashboardData();
	}

	private async void LoadDashboardData() {
		// Simulate loading data
		await Task.Delay(1000);

		TotalProductsLabel.Text = "25";
		LowStockLabel.Text = "3";
		RecentMovementsLabel.Text = "12";
		ActiveSuppliersLabel.Text = "8";
	}

	private async void OnAddProductClicked(object sender, EventArgs e) {
		await DisplayAlert("Add Product", "Navigate to add product page", "OK");
	}

	private async void OnStockAdjustmentClicked(object sender, EventArgs e) {
		await DisplayAlert("Stock Adjustment", "Navigate to stock adjustment page", "OK");
	}

	private async void OnViewReportsClicked(object sender, EventArgs e) {
		await DisplayAlert("Reports", "Navigate to reports page", "OK");
	}
}
