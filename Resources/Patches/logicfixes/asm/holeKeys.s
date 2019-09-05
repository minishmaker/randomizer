.thumb
@check if the area and room match
ldr	r0,=#0x3000BF0
ldrb	r1,[r0,#4]
cmp	r1,#0x58
bne	vanilla
@check if left side
ldrb	r1,[r0,#5]
cmp	r1,#0x04
beq	left
@check if right side
cmp	r1,#0x02
beq	right
b	vanilla

vanilla:
mov	r1,#0x53
mov	r2,#0
b	end

left:
ldr	r3,=#0x80FC46B
ldrb	r1,[r3]
ldrb	r2,[r3,#1]
b	end

right:
ldr	r3,=#0x80FC4AB
ldrb	r1,[r3]
ldrb	r2,[r3,#1]
b	end

end:
mov	r0,#0
ldr	r3,=#0x80A217C
mov	lr,r3
.short	0xF800
ldr	r3,=#0x8058BAB
bx	r3
