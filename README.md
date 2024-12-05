# C-SimpleRPG
An RPG created using C# and WPF implementing the ViewModel design logic.

C#とWPF(Windows Presentation Foundation) XAMLを使用してUIの表示を行い、ゲームエンジンを作成したプロジェクト。

![アプリ画面](https://github.com/SEFWHITE-Dev/C-Sharp-SimpleRPG/tree/main/Images/01.png?raw=true)

![マップ映像](https://github.com/SEFWHITE-Dev/C-Sharp-SimpleRPG/tree/main/Images/Map.png?raw=true)
マップでは９か所に移動が可能。座標によってエンカウントするエンティティが変わる。

エンカウント種類:
・ショップ
・敵
・クエスト

プロジェクト構成:
WPFUIプロジェクトでは、XAMLで「タグ」を使い「グリッド」を制作。
Engineプロジェクトでは、C#でViewModelクラスを生成。
View（MainWindow.xaml）とModel（Player.cs, Monster.cs, など）がコミュニケーションを取れるようにViewModel.csを生成。
後ほどViewModelを使用して、自動テストも制作。

ViewModelでは、他にも「モデル」（クラス）を管理する用に構成されている。
例えば戦闘では、プレイヤーとモンスターがコミュニケーションを取る事が必要となる。

Viewはプロパティの変化を把握できないため、INotifyPropertyChangedのインターフェースを実装する。
public class BaseNotificationClass : INotifyPropertyChanged
クラスがINotifyPropertyChangedを実装すると、そのプロパティはPropertyChanged「イベント」を「発生」させる。
Viewはそのイベントを 「リスン 」し、変更の通知を受け取るとUIを更新する。
View のようなイベントハンドラにサブスクライブされる可能性のあるものに対して、 PropertyChanged イベントを発生させるコードを追加する必要がある。
