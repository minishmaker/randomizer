.thumb
ldr	r0,=#0x3001160
mov	r2,#6
push	{r4-r7,lr}
mov	r4,r0
ldr	r5,=#0x3003F80
mov	r7,r2

mov	r0,#0x42
mov	r1,#1
mov	r2,#0
ldr	r3,=#0x80A217C
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	end

str	r4,[r0,#0x54]
ldr	r0,[r5,#0x30]
ldr	r1,=#0x400
orr	r0,r1
str	r0,[r5,#0x30]
mov	r0,#0x10
eor	r0,r7
lsr	r0,#2
strb	r0,[r4,#0x14]

mov	r1,r4
add	r1,#0x45
mov	r0,#0xC
strb	r0,[r1]
sub	r1,#3
strb	r0,[r1]
sub	r1,#5
mov	r0,#0x1E
strb	r0,[r1]
add	r1,#9
ldr	r0,=#0x180
strh	r0,[r1]

ldr	r0,=#0x124
ldr	r3,=#0x80A2A80
mov	lr,r3
.short	0xF800

end:
pop	{r4-r7,pc}
