.thumb
push	{r4-r7,lr}
ldr	r4,=#21502799
cmp	r0,r4
blo	notmax
mov	r0,r4
notmax:
mov	r4,r0	@number
mov	r5,r1	@x
mov	r6,r2	@y
push	{r3}	@offset
ldr	r7,=#0x203F000

@draw hours
mov	r0,r4
ldr	r1,=#215028
swi	#6
mov	r4,r1
mov	r1,#10
swi	#6
mov	r2,#0x30
add	r0,r2
add	r1,r2
strb	r0,[r7,#0]
strb	r1,[r7,#1]

@draw :
mov	r0,#0x3A
strb	r0,[r7,#2]

@draw minutes
mov	r0,r4
mov	r1,#10
mul	r0,r1
ldr	r1,=#35838
swi	#6
mov	r4,r1
mov	r1,#10
swi	#6
mov	r2,#0x30
add	r0,r2
add	r1,r2
strb	r0,[r7,#3]
strb	r1,[r7,#4]

@draw :
mov	r0,#0x3A
strb	r0,[r7,#5]

@draw seconds
mov	r0,r4
mov	r1,#10
mul	r0,r1
ldr	r1,=#5973
swi	#6
mov	r4,r1
mov	r1,#10
swi	#6
mov	r2,#0x30
add	r0,r2
add	r1,r2
strb	r0,[r7,#6]
strb	r1,[r7,#7]

@terminator
mov	r0,#0
strb	r0,[r7,#8]

endDraw:
mov	r0,r7
mov	r1,r5
mov	r2,r6
ldr	r3,drawText
mov	lr,r3
pop	{r3}
.short	0xF800
pop	{r4-r7,pc}
.align
.ltorg
drawText:
