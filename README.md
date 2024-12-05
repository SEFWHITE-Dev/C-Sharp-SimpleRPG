# C-SimpleRPG
An RPG created using C# and WPF implementing the ViewModel design logic.

C#とWPF(Windows Presentation Foundation) XAMLを使用してUIの表示を行い、ゲームエンジンを作成したプロジェクト。

![アプリ画面](https://github.com/SEFWHITE-Dev/C-Sharp-SimpleRPG/tree/main/Images/01.png?raw=true)</br>

<h2>MVVM デザインパターン</h2>
<h3>プロジェクト構成: </h3>
WPF_UIソリューションは主に３つのプロジェクトがある。</br>
・WPF_UIプロジェクトでは、XAMLで「タグ」を使い「グリッド」、「ボタン」などを制作。</br>
・Engineプロジェクトでは、C#でViewModel(GameSession)などのクラスを生成。</br>
・TestGameEngineプロジェクトでは、自動テストを制作。</br>
</br>
View（MainWindow.xaml）とModel（Player.cs, Monster.cs, など）がコミュニケーションを取れるようにViewModel.csを生成。</br>
後ほどViewModelを使用して、自動テストも制作。</br>
</br>
ViewModelでは、他にも「モデル」（クラス）を管理する用に構成されている。</br>
例えば戦闘では、プレイヤーとモンスターがコミュニケーションを取る事が必要となる。</br>
</br>
</br>
Viewはプロパティの変化を把握できないため、INotifyPropertyChangedのインターフェースを実装する。</br>

    public class BaseNotificationClass : INotifyPropertyChanged

クラスがBaseNotificationClassを実装(継承)すると、そのクラス内のプロパティはPropertyChanged「イベント」を「発生」させる事が可能。</br>
</br>
</br>
Viewはそのイベントを 「リスン 」し、変更の通知を受け取るとUIを更新する。</br>
Viewのようなイベントハンドラにサブスクライブされる可能性のあるものに対して、</br>
PropertyChanged イベントを発生させるコードを追加する必要がある。</br>
</br>
こうしてView-Modelデザインパターンを使用した構成をプロジェクトの土台とする。
</br></br>


<h2>Factory Method デザインパターン</h2>
GameSessionコンストラクタ内で新しいオブジェクト（Player, Location, Monster, Itemなど）を初期化し、</br>
プロパティとして保存する事が可能だが、各クラスを初期化する度に多くのコードを追加する必要となる。</br>
このプロジェクトでは、ItemFactoryクラスを生成して、その中に全Itemオブジェクトを初期化して、GameSessionクラス内では、</br>
ItemFactoryクラスを初期化することで、コードが整理しやすくなる。</br>
</br>
Factoriesフォルダー内には、ItemFactory、MonsterFactory、QuestFactory、RecipeFactory、WorldFactory、などのinternal staticクラスがある。</br>
</br></br>

![マップ映像](https://github.com/SEFWHITE-Dev/C-Sharp-SimpleRPG/tree/main/Images/Map.png?raw=true)</br>
マップでは９か所に移動が可能。座標によってエンカウントするエンティティが変わる。</br>



エンカウント種類:</br>
・ショップ</br>
・敵</br>
・クエスト</br>
