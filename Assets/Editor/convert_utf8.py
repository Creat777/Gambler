import os
import chardet

def convert_to_utf8(directories):
    # 여러 디렉토리에 대해 반복 처리
    for directory in directories:
        print(f"디렉토리: {directory} 처리 시작...")
        
        # 지정된 디렉토리에서 모든 CSV 파일을 찾기
        for filename in os.listdir(directory):
            if filename.endswith(".csv"):  # .csv는 작은 따옴표로 감싸야 합니다
                file_path = os.path.join(directory, filename)

                # 파일의 현재 인코딩 확인 (chardet를 사용)
                with open(file_path, 'rb') as file:
                    raw_data = file.read()
                    result = chardet.detect(raw_data)
                    current_encoding = result['encoding']
                
                # 만약 인코딩을 감지하지 못했다면 (None이 반환되었을 경우), 건너뛰기
                if current_encoding is None:
                    print(f"{filename}의 인코딩을 감지할 수 없습니다. 건너뛰기.")
                    continue

                # 만약 이미 UTF-8이면 건너뛰기
                if current_encoding.lower() == 'utf-8':
                    print(f"{filename}는 이미 UTF-8 인코딩입니다.")
                    continue
                
                # UTF-8로 변환하여 저장
                try:
                    with open(file_path, 'r', encoding=current_encoding, errors='ignore') as file:
                        content = file.read()

                    with open(file_path, 'w', encoding='utf-8') as file:
                        file.write(content)
                    print(f"{filename}의 인코딩을 UTF-8로 변환 완료.")
                except Exception as e:
                    print(f"{filename}의 인코딩을 변환하는 중 오류가 발생했습니다: {e}")

# 여러 디렉토리 경로 리스트
directories = [
    r"D:\YoonChangJun\Gambler\Assets\Resources\CSV\TextScript\NoneInteractable",
    r"D:\YoonChangJun\Gambler\Assets\Resources\CSV\TextScript\Interactable",
    r"D:\YoonChangJun\Gambler\Assets\Resources\CSV\TextScript"
]

convert_to_utf8(directories)

# 사용자가 아무 키나 누를 때까지 대기
input("\n 모든 작업이 완료되었습니다. 종료하려면 Enter를 누르세요...")
